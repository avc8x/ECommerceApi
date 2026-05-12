import React from 'react';
import './TopBar.css';

export default function TopBar() {
  return (
    <div className="topbar">
      <span className="topbar-left">📍 Store Location</span>
      <div className="topbar-right">
        <a href="#">USD 🇺🇸</a>
        <a href="#">Eng</a>
        <a href="#">Sign In / Sign Up</a>
      </div>
    </div>
  );
}
