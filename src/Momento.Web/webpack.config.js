"use strict";

module.exports = {
    entry: {
        components: "./ReactSource/Navigation/ExposeComponents.js",
    },

    output: {
        filename: "../wwwroot/js/ReactApps/Navigation/[name].bundle.js"
    },

    devtool: 'source-map',

    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                loaders: ['babel-loader'],
                exclude: /node_modules/,
            }
        ]
    },
    mode: 'development',
    watch: true,
};