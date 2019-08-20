const winston = require('winston');
const fs = require('fs');

if (!fs.existsSync('./logs')) {
  fs.mkdirSync('logs');
}

// eslint-disable-next-line new-cap
module.exports = new winston.createLogger({
  transports: [
    new winston.transports.File({
      level: 'info',
      filename: './logs/invalidLicenses.log',
      maxsize: 100000,
      maxFiles: 10,
    }),
  ],
});
