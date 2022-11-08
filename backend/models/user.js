const mongoose = require('mongoose')

const userShema = mongoose.Schema({
    email : {
        type : String,
        required : [true, 'Please add email']
    },
    password : {
        type : String,
        required : [true, 'Please add password.']
    },
    images : {
        type : [String],
        required : [false]
    }
}, {
    timestamps : true
})

module.exports = mongoose.model('users', userShema)