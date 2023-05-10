import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.scss'
import {createHashRouter, RouterProvider} from "react-router-dom";
import Register from "./UIs/Authentication/Register";
import Login from "./UIs/Authentication/Login";
import PasswordForgotten from "./UIs/Authentication/PasswordForgotten";
import Chat from "./UIs/Chat/Chat";

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
    },
    {
        path: "/Chat",
        element: <Chat />
    }
]);

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  // <React.StrictMode>
      <RouterProvider router={router} />
  // </React.StrictMode>,
)
