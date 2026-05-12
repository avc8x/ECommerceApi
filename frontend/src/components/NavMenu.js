import React, { useState } from 'react';
import './NavMenu.css';

const LINKS = ["Today's Deals", "Become a Seller", "Customer Service", "About Us", "Contact Us"];

export default function NavMenu({ categories }) {
  const [open, setOpen] = useState(false);

  return (
    <div className="navmenu">
      <div className="cat-wrap">
        <button className="cat-btn" onClick={() => setOpen(o => !o)}>
          <span className="hamburger">☰</span> All Category
        </button>
        {open && (
          <div className="cat-dropdown">
            <ul>
              {categories.length > 0
                ? categories.map(c => (
                    <li key={c.id}>
                      <a href="#" onClick={() => setOpen(false)}>
                        <span className="cat-dot" /> {c.title}
                        {c.children?.length > 0 && <span className="cat-arrow">›</span>}
                      </a>
                    </li>
                  ))
                : <li><a href="#">No categories yet</a></li>
              }
            </ul>
          </div>
        )}
      </div>
      <ul className="navmenu-links">
        {LINKS.map(l => <li key={l}><a href="#">{l}</a></li>)}
      </ul>
    </div>
  );
}
