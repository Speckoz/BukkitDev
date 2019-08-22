const connection = require('../infra/ConnectionFactory');

function PluginDAO() {
  this.connection = connection;
}

PluginDAO.prototype.getPlugin = function getPlugin(id) {
  return new Promise((resolve, reject) => {
    this.connection.query(
      'SELECT * FROM pluginlist WHERE ID = ?',
      [id],
      (err, result) => {
        if (err) return reject(err);

        return resolve(result[0]);
      }
    );
  });
};

// eslint-disable-next-line func-names
module.exports = new PluginDAO();
