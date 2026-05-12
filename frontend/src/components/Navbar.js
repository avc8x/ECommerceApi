import React, { useState } from 'react';
import './Navbar.css';

export default function Navbar() {
  const [search, setSearch] = useState('');
  const [cat, setCat] = useState('All');

  return (
    <nav className="navbar">
      <a href="/" className="logo">
        <span className="logo-icon">LOGO</span>
        <span className="logo-text">BRAND</span>
      </a>

      <div className="search-wrap">
        <select value={cat} onChange={e => setCat(e.target.value)} className="search-select">
          <option>All</option>
          <option>Fashion</option>
          <option>Electronics</option>
          <option>Books</option>
          <option>Sports</option>
        </select>
        <input
          className="search-input"
          type="text"
          placeholder="Search products..."
          value={search}
          onChange={e => setSearch(e.target.value)}
        />
        <button className="search-btn">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
            <circle cx="11" cy="11" r="8"/><path d="m21 21-4.35-4.35"/>
          </svg>
        </button>
      </div>

      <div className="nav-icons">
        <button className="nav-icon-btn">
          <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/>
          </svg>
        </button>
        <button className="nav-icon-btn cart-btn">
          <div className="cart-icon-wrap">
            <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <circle cx="9" cy="21" r="1"/><circle cx="20" cy="21" r="1"/>
              <path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6"/>
            </svg>
            <span className="badge">3</span>
          </div>
          <span className="cart-label">Shopping cart:<br/><strong>$200.00</strong></span>
        </button>
      </div>
    </nav>
  );
}
