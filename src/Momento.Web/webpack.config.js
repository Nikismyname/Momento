"use strict";

module.exports = {
    entry: {
        components: "./ReactSource/Navigation/ExposeComponents.js",
    },

    output: {
        filename: "../wwwroot/js/ReactApps/Navigation/[name].bundle.js"
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

    devtool: 'source-map',
    mode: 'development',
    watch: true,
};