const express = require('express')
const router = express.Router()
const { getUser, register, login, updateDetails } = require('../controllers/user')

router.get('/:id', getUser)
router.post('/register', register)
router.post('/login', login)
router.put('/:id', updateDetails)

module.exports = router;