import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.scss'
import {createHashRouter, RouterProvider} from "react-router-dom";
import Register from "./UIs/Register";
import Login from "./UIs/Login";
import PasswordForgotten from "./UIs/PasswordForgotten";

const router = createHashRouter([
    {
        path: "/register",
        element: <Register />
    },
    {
        path: "/login",
        element: <Login />
    },
    {
        path: "/PasswordForgotten",
        element: <PasswordForgotten />
    }
]);

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
      <RouterProvider router={router} />
  </React.StrictMode>,
)
