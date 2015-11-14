# Abyss State Server - RESTful service.

from flask import Flask, abort

app = Flask(__name__)

accounts = {
    "aggpys" : 512,
    "test" : 0,
    "dev" : 5210525
}

# REST API: retrieves the in-game points for the specified account.
@app.route('/api/points/<account>', methods=['GET'])
def get_points(account):
    if (account in accounts):
        return str(accounts[account])
    abort(404)

# The main entry point for the server application.
if __name__ == '__main__':
    app.run(debug=True)