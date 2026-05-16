import React, { useState, useEffect, useRef } from 'react';
import './NavMenu.css';

const LINKS = ["Today's Deals", "Become a Seller", "Customer Service", "About Us", "Contact Us"];

export default function NavMenu({ categories }) {
  const [open, setOpen] = useState(false);
  const ref = useRef(null);

  // Close dropdown when clicking outside
  useEffect(() => {
    function handleClick(e) {
      if (ref.current && !ref.current.contains(e.target)) {
        setOpen(false);
      }
    }
    document.addEventListener('mousedown', handleClick);
    return () => document.removeEventListener('mousedown', handleClick);
  }, []);

  return (
    <div className="navmenu">
      <div className="cat-wrap" ref={ref}>
        <button className="cat-btn" onClick={() => setOpen(o => !o)}>
          <span className="hamburger">☰</span> All Category
        </button>
        {open && (
          <div className="cat-dropdown">
            <ul>
              {categories.length > 0
                ? categories.map(c => (
                    <li key={c.id}>
                      <a href="#" onClick={(e) => { e.preventDefault(); setOpen(false); }}>
                        <span className="cat-dot" /> {c.title}
                        {c.children?.length > 0 && <span className="cat-arrow">›</span>}
                      </a>
                    </li>
                  ))
                : [
                    'Fashion', 'Electronics', 'Books', 'Sports',
                    'Furniture', 'Decor', 'Skin Care', 'Perfumes'
                  ].map(name => (
                    <li key={name}>
                      <a href="#" onClick={(e) => { e.preventDefault(); setOpen(false); }}>
                        <span className="cat-dot" /> {name}
                      </a>
                    </li>
                  ))
              }
            </ul>
          </div>
        )}
      </div>
      <ul className="navmenu-links">
        {LINKS.map(l => (
          <li key={l}>
            <a href="#" onClick={e => e.preventDefault()}>{l}</a>
          </li>
        ))}
      </ul>
    </div>
  );
}
