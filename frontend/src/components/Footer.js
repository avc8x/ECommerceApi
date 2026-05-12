import React from 'react';
import './Footer.css';

export default function Footer() {
  return (
    <footer className="footer">
      <div className="footer-grid">
        <div className="footer-col footer-about">
          <h4>About Us</h4>
          <p>Your premier destination for online shopping, where we bring together quality and variety in one place. We always strive to offer the best products at the best prices.</p>
          <h5>Shop on the go</h5>
          <div className="store-btns">
            <a href="#" className="store-btn">▶ Google Play</a>
            <a href="#" className="store-btn"> App Store</a>
          </div>
        </div>
        <div className="footer-col">
          <h4>Join Us</h4>
          <ul>
            {['Become a Vendor','For Delivery Drivers','Stores List','Privacy Policy'].map(l =>
              <li key={l}><a href="#">{l}</a></li>
            )}
          </ul>
        </div>
        <div className="footer-col">
          <h4>Helps</h4>
          <ul>
            {['Contact','Faqs','Terms & Condition','Privacy Policy'].map(l =>
              <li key={l}><a href="#">{l}</a></li>
            )}
          </ul>
        </div>
        <div className="footer-col">
          <h4>Account</h4>
          <ul>
            {['My Account','Order History','Wishlist','Account Settings'].map(l =>
              <li key={l}><a href="#">{l}</a></li>
            )}
          </ul>
        </div>
      </div>

      <div className="footer-bottom">
        <div className="social-links">
          {['f','t','p','in'].map(s => <a key={s} href="#">{s}</a>)}
        </div>
        <span>eCommerce &copy; 2025. All Rights Reserved</span>
        <div className="payment-icons">
          {['APPLE PAY','VISA','DISC','MC','SECURE'].map(p =>
            <div key={p} className="payment-icon">{p}</div>
          )}
        </div>
      </div>
    </footer>
  );
}
