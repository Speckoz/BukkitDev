const express = require('express');

const routes = express.Router();
const LicencaController = require('./app/controllers/LicencaController');
const PaymentController = require('./app/controllers/PaymentController');

// Licenca
routes.get('/licenca/:licenca', LicencaController.verifyLic);

// Mercado Pago
routes.post('/CreatePayment', PaymentController.createPayment);

module.exports = routes;
