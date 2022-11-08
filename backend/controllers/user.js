const asyncHandler = require('express-async-handler')
const User = require('../models/user')
const bcrypt = require('bcryptjs')
const jwt = require('jsonwebtoken')
const cloudinary = require('../utils/cloudinary/cloudinary')


const getUser = asyncHandler(async (req, res) => {
    const userId = req.params.id;
    const user = await User.findById(userId).select('-password')
    res.status(200)
        .json(user)
    // throw new Error('Something went wrong!')
})

const login = asyncHandler(async (req, res) => {
    const { email, password } = req.body;

    const user = await User.findOne({ email })
    // console.log(user)
    if(user && (await bcrypt.compare(password, user.password))){
        res.status(200)
            .json({
                userId : user._id,
                email : user.email,
                token : generateToken(user._id, user.email) 
            })
    }else{
        res.status(400)
        throw new Error('Invalid credentials')
    }

    
})

const register = asyncHandler(async (req, res) => {
    const { email, password } = req.body
    if(!email || !password){
        res.status(400)
        throw new Error('Email and password are required.')
    }
    const user = await User.findOne({ email : email })
    if(user) {
        res.status(400)
        throw new Error('Email is already exist')
    }
    const salt = await bcrypt.genSalt(10)
    const hashedPassword = await bcrypt.hash(password, salt)
    const createdUser = await User.create({
        email,
        password : hashedPassword
    })

    res.status(200)
        .json({
            userId : createdUser._id,
            email : createdUser.email,
            token : generateToken(createdUser._id, createdUser.email)
        })
})

const updateDetails = asyncHandler(async (req, res) => {
    const userId = req.params.id
    const files = req.files
    const { email } = req.body;
    let uploadedFiles = []
    const user = await User.findById(userId)

    if(!email){
        res.status(400)
        throw new Error('Failed to update')
    }

    if(!user){
        res.status(404)
        throw new Error('User not found.')
    }

    if(files){

        for(let img of files.imgs){
            const result = await cloudinary.uploader.upload(img.tempFilePath, {
                folder : 'user_images',
            })
            uploadedFiles.push(result.secure_url)
        }
    }
    const update = files ? { email, images : uploadedFiles } : { email }

    await User.findByIdAndUpdate({ _id : userId }, update)
    
    
    res.json({
        message : 'Successfuly updated'
    })
})

const generateToken = (id, email) => {
    return jwt.sign({
        id,
        email
    }, process.env.SECRET, {
        expiresIn : '1d'
    })
}

module.exports = {
    getUser,
    register,
    login,
    updateDetails
}