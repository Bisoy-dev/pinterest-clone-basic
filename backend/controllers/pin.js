const Pin = require('../models/pin')
const User = require('../models/user')
const asyncHandler = require('express-async-handler')
const cloudinary = require('../utils/cloudinary/cloudinary')

const create = asyncHandler(async (req, res) => {
    const file  = req.files
    const { userId, title, description, link } = req.body;
    const user = await User.findById(userId)
    if(!user){
        res.status(400)
        throw new Error('User not found!')
    }

    if(!title){
        res.status(400)
        throw new Error('Please add title')
    }

    const result = await cloudinary.uploader.upload(file.img.tempFilePath, {
        folder : 'pins'
    })

    const pin = {
        user_id : userId,
        title,
        description,
        distination_link : link,
        img : result.secure_url,
        date : {
            createdAt : new Date(),
            updatedAt : new Date()
        }
    }

    const createdPin = await Pin.create(pin)
    res.status(200)
        .json({
            message : 'Pin Successfully created',
            content : createdPin
        })


})

const update = asyncHandler(async (req, res) => {
    const pinId = req.params.id
    const file = req.files
    const { title, description, link } = req.body
    if(!title){
        res.status(400)
        throw new Error('Please add a title.')
    }

    const pin = await Pin.findById(pinId)
    if(!pin){
        res.status(404)
        throw new Error('Pin not found')
    }

    const img = file ? await imageUpload(file) : pin.img

    const updatedPin = {
        title,
        description,
        link,
        img,
        date : {
            updatedAt : new Date(),
            createdAt : pin.date.createdAt
        }
    }

    await Pin.findByIdAndUpdate({ _id : pinId }, updatedPin)

    res.status(200)
        .json({
            message : 'Pin successfully updated.'
        })
})

const getUserPins = asyncHandler(async (req, res) => {
    const pins = await Pin.find({ user_id : req.params.userId })
    res.status(200)
        .json(pins)
})

const deletePin = asyncHandler(async (req, res) => {
    const pin = await Pin.findById(req.params.id)
    if(!pin){
        res.status(404)
        throw new Error('Pin not found!')
    }
    await Pin.findByIdAndDelete(req.params.id)
    res.status(200)
        .json({
            message : 'Deleted successfully.'
        })
})

const like = asyncHandler(async (req, res) => {
    // console.log('hitted')
    // end point that to be fix!
    const { pinId, userId } = req.body;
    if(!pinId || !userId){
        res.status(400)
        throw new Error('Failed to like the pin.')
    }

    const user = await User.findById(userId)
    if(!user){
        res.status(404)
        throw new Error('User not found!')
    }
    
    const pin = await Pin.findById(pinId)
    if(!pin){
        res.status(400)
        throw new Error('Pin not found!')
    }
    let pinLikes = new Set(pin.likes)
    const contain = pinLikes.has(user.id)
    console.log(pinLikes)
    // if(pinLikes.has(userId)){
    //     pinLikes.delete(userId)
    //     await Pin.updateOne({ _id : pinId }, 
    //         { $addToSet : { likes : [...userId] } })
    // }else{
    //     pinLikes.add(userId)
    //     await Pin.updateOne({ _id : pinId }, 
    //         { $addToSet : { likes : [...userId] } })
    // }
    // await Pin.updateOne({ _id : pinId }, { $push : { "likes" : userId } })
    // await Pin.updateOne({ _id : pinId }, 
    //     { $addToSet : { likes : [...userId] } })
    
    
    console.log(Array.from(pinLikes))
    //await pin.save();
    res.status(200)
        .json({
            message : 'Liked',
            result : pin.likes,
            contain
        })
})

const imageUpload = async (file) => {
    const result = await cloudinary.uploader.upload(file.img.tempFilePath, {
        folder : 'pins'
    })
    return result.secure_url
}


module.exports = {
    create,
    update,
    getUserPins,
    deletePin,
    like
}