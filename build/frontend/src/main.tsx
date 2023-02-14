import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.scss'
import {createHashRouter, RouterProvider} from "react-router-dom";
import Register from "./UIs/Register";

const router = createHashRouter([
    {
        path: "/register",
        element: <Register />
    },
]);

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
      <RouterProvider router={router} />
  </React.StrictMode>,
)
