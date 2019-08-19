const express = require('express');

const routes = express.Router();
const LicencaController = require('./app/controllers/LicencaController');

routes.get('/licenca/:licenca', LicencaController.verifyLic);

module.exports = routes;
