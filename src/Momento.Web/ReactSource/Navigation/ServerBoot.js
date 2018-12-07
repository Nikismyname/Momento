//import * as React from 'react';
import * as prerendering from 'aspnet-prerendering';
//import { createStore, applyMiddleware } from 'redux';
//import { Provider } from 'react-redux';
//import thunk from 'redux-thunk';
//import { helloWorld_reducers } from './redux-reducers';
//import { HelloWorld } from './HelloWorld';
import { renderToString } from 'react-dom/server';
import Counter3 from './Components/Counter3';

let ServerRendering = prerendering.createServerRenderer(params => {
    return new Promise((resolve, reject) => {

        var app = <Counter3 />;

        renderToString(app);

        params.domainTasks.then(() => {
            resolve({
                html: renderToString(app)
            });
        });
    });
});

export default ServerRendering;

//import * as prerendering from 'aspnet-prerendering';
//import Counter3 from './Components/Counter3';
//import { renderToString } from 'react-dom/server';

//var SR = prerendering.createServerRenderer((params => {
//    return new Promise((resolve, reject) => {

//        var app = <Counter3 number={params.number} />;
//        renderToString(app); // This kick off any async tasks started by React components

//        // any async task (has a Promise) should call addTask() to add to domainTasks.
//        // only do the actual rendering when all async tasks are done.
//        params.domainTasks.then(() => {
//            resolve({
//                html: renderToString(app)
//            });
//        }); // Also propagate any errors back into the host application
//    });
//}));

//export default SR;


//const prerendering = require('aspnet-prerendering');

//let SR = prerendering.createServerRenderer(params => {
//    return new Promise((resolve, reject) => {
//        const result = `
//            <h1>Hello from JS</h1>
//            <p>Current time in Node is: ${new Date()}</p>
//            <p>Request path is: ${params.location.path}</p>
//            <p>Absolute URL is: ${params.absoluteUrl}</p>
//        `;
//        resolve({html: result})
//    });
//});

//module.exports = SR;