const express = require('express')
const router = express.Router()
const { create, update, getUserPins, deletePin, like } = require('../controllers/pin')

router.put('/like', like)
router.post('/', create)
router.put('/:id', update)
router.get('/:userId', getUserPins)
router.delete('/:id', deletePin)


module.exports = router