import React, { useState } from 'react';
import './Newsletter.css';

export default function Newsletter() {
  const [email, setEmail] = useState('');
  const [sent, setSent] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (email) { setSent(true); setEmail(''); }
  };

  return (
    <div className="newsletter">
      <div className="newsletter-logo">BRAND</div>
      <div className="newsletter-text">
        <h3>Subscribe our Newsletter</h3>
        <p>Pellentesque eu nibh eget mauris congue mattis matti.</p>
      </div>
      {sent
        ? <p className="newsletter-thanks">✅ Thanks for subscribing!</p>
        : (
          <form className="newsletter-form" onSubmit={handleSubmit}>
            <input
              type="email"
              placeholder="Your email address"
              value={email}
              onChange={e => setEmail(e.target.value)}
              required
            />
            <button type="submit">Subscribe</button>
          </form>
        )
      }
    </div>
  );
}
