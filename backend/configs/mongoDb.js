const mongoose = require('mongoose')

const connectToMongo = async () => {
    try {
        const conn = await mongoose.connect(process.env.MONGO_CONNECTION)
        console.log('Connected to mongodb successfully!')
    } catch (error) {
        console.log(error)
    }
}

module.exports = connectToMongo;