require('dotenv').config()
const express = require('express')
const connectToMongo = require('./configs/mongoDb')
const cors = require('cors')
const fileUpload = require('express-fileupload')
const { errorHandler } = require('./middlewares/errorHandler')

connectToMongo();

const app = express()
const port = process.env.PORT || 3000

// middlewares
app.use(cors())
app.use(express.json())
app.use(express.urlencoded({ extended : false }))
app.use(fileUpload({ useTempFiles : true }))

app.use('/api/user/', require('./routers/user'))
app.use('/api/pin/', require('./routers/pin'))

// error handler
app.use(errorHandler)

app.listen(port, () => {
    console.log('Server running on ' + port)
})