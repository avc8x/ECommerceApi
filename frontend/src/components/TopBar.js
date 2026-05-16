import React from 'react';
import './TopBar.css';

export default function TopBar() {
  return (
    <div className="topbar">
      <span className="topbar-left">📍 Store Location</span>
      <div className="topbar-right">
        <a href="#" onClick={(e) => e.preventDefault()}>USD 🇺🇸</a>
        <a href="#" onClick={(e) => e.preventDefault()}>Eng</a>
        <a href="#" onClick={(e) => e.preventDefault()}>Sign In / Sign Up</a>
      </div>
    </div>
  );
}
