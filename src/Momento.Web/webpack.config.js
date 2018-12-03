"use strict";

module.exports = {
    entry: "./ReactSource/Navidation/App.jsx",
    output: {
        filename: "../wwwroot/js/ReactApps/Navigation.js"
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                loaders: ['babel-loader'],
                exclude: /node_modules/,
            }
        ]
    },
    mode: 'development'
};