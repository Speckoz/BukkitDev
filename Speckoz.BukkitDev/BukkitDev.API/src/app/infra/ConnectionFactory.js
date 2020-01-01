const mysql = require('mysql');

const { MYSQL_USER, MYSQL_PASSWORD, MYSQL_URL } = process.env;

function createDBConnection() {
  return mysql.createConnection({
    host: MYSQL_URL,
    user: MYSQL_USER,
    password: MYSQL_PASSWORD,
    database: 'bukkitdev',
  });
}

module.exports = createDBConnection();
