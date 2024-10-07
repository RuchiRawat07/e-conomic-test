import React from "react";
import { useNavigate } from "react-router-dom";

interface NavigateButtonProps {
    route: string;
    label: string;
    className: string;
}

export default function NavigateButton({ route, label, className }: NavigateButtonProps) {
  
    const navigate = useNavigate();
    const handleClick = () => {
        navigate(route);
    };

    return (
        <button 
        onClick={handleClick}
        className={className}>
            {label}
        </button>
    );
}
