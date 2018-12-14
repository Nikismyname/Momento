import "@babel/polyfill";

import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';
import Helmet from 'react-helmet';

import RootElement from './Components/AppRouting';

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;
global.Helmet = Helmet;
global.RootElement = RootElement;
