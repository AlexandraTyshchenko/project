// Bootstrap CSS
import "bootstrap/dist/css/bootstrap.min.css";
// Bootstrap Bundle JS
import "bootstrap/dist/js/bootstrap.bundle.min";
import Header from './components/layout/Header';
import Footer from './components/layout/Footer';
import SideBar from './components/layout/Sidebar';
import Openbar from './components/layout/Openbar';
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css';

import { getAuth } from "./services/auth";

const root = ReactDOM.createRoot(document.getElementById('root'));
const authData = getAuth()
root.render(
  <React.StrictMode>
    
    <Header user = {authData?.user} authorized = {authData !== null} />
    {authData !== null && (
      <>
        <Openbar />
        <SideBar />
      </>
    )}
    
    <App />
    <Footer />
  </React.StrictMode>
);

