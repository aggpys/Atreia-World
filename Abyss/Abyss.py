# Abyss State Server - RESTful service.

import os
import MySQLdb

from flask import Flask
from flask import abort, request
from flask_cache import Cache

API_VS = 'Abyss API 0.0.3a'

MYSQL_USER = os.getenv('MYSQL_DEFAULT_USER', 'root')
MYSQL_PASSWORD = os.getenv('MYSQL_DEFAULT_PASSWORD', '')
MYSQL_HOST = os.getenv('MYSQL_DEFAULT_HOST', 'localhost')
MYSQL_DBNAME = os.getenv('MYSQL_DEFAULT_DATABASE')

QUERY_GET_ACCOUNT_ID = "SELECT id FROM account_data WHERE name=%s AND password=%s"
QUERY_GET_POINTS = "SELECT toll FROM account_tolls WHERE id=%s"

abyss_app = Flask(__name__)
abyss_cache = Cache(abyss_app, config={'CACHE_TYPE': 'simple'})
db_connection = MySQLdb.connect(MYSQL_HOST, MYSQL_USER, MYSQL_PASSWORD, MYSQL_DBNAME)

# ------------------------------------------------

# REST API: returns the API version string.
@abyss_app.route('/')
def getapiv():
    return API_VS

# REST API: returns the total amount of points for the specified account.
@abyss_app.route('/api/points/<account>')
@abyss_cache.memoize(60)
def getpoints(account):
    password = request.args.get('p')
    
    if password == None:
        abort(404)

    cursor = db_connection.cursor()
    
    try:
        cursor.execute(QUERY_GET_ACCOUNT_ID, (account, password))
        data = cursor.fetchone()

        if data == None:
            abort(404)

        id = data[0]
        cursor.execute(QUERY_GET_POINTS, (id,))
        points_data = cursor.fetchone()
    
        if points_data == None:
            abort(404)

        return str(points_data[0])
    except:
        abort(404)

# REST API: returns the server status code.
@abyss_app.route('/api/status')
@abyss_cache.memoize(120)
def getstatus():
    return str(0)

# ------------------------------------------------
# The main entry point for the server application.
# ------------------------------------------------
if __name__ == '__main__':
    abyss_app.run(debug=True)