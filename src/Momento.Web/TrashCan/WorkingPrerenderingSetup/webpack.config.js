"use strict";

module.exports = {
    entry: {
        server: "./ReactSource/Navigation/ServerBoot.js",
        client: "./ReactSource/Navigation/Client.js",
        components: "./ReactSource/Navigation/ExposeComponents.js",
        Counter3: "./ReactSource/Navigation/Components/Counter3.js",
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