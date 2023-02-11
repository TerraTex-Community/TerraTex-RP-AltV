import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import './index.css'
import {createHashRouter, RouterProvider} from "react-router-dom";
import App2 from "./App2";

const router = createHashRouter([
    {
        path: "/",
        element: <App />
    },
    {
        path: "/test",
        element: <App2 />
    },
]);

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    {/*<App />*/}
      <RouterProvider router={router} />
  </React.StrictMode>,
)
