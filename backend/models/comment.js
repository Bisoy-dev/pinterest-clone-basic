const mongoose = require('mongoose')

const commentSchema = mongoose.Schema({
    user_id : {
        type : mongoose.Types.ObjectId,
        required : [true],
        ref : 'users'
    },
    text : {
        type : String,
        required : [true, 'Please add a comment']
    },
    date : {
        createdAt : Date,
        updatedAt : Date
    }
}, {
    timestamps : true
})

module.exports = mongoose.model("comments", commentSchema);