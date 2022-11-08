const mongoose = require('mongoose')


const pinSchema = mongoose.Schema({
    user_id : {
        type : mongoose.Types.ObjectId,
        required : [true],
        ref : 'users'
    },
    img : {
        type : String,
        required : [true, 'Please add photo']
    },
    title : {
        type : String,
        required : [true, 'Add title']
    },
    description : {
        type : String
    },
    distination_link : {
        type : String
    },
    likes : {
        type : [mongoose.Types.ObjectId]
    },
    comments : {
        type : [mongoose.Types.ObjectId],
        ref : 'comments'
    },
    date : {
        createdAt : Date,
        updatedAt : Date
    }
})

module.exports = mongoose.model("pins", pinSchema)