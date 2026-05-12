import React, { useState, useEffect, useCallback } from 'react';
import './Swiper.css';

const DEMO = [{
  topText: 'WELCOME TO OUR STORE',
  bigTitle: 'New & Casual',
  highlightedTitleNormal: 'Collection',
  highlightedTitleColor: 'Trendy',
  highlightedTitleBold: null,
  bottomText: 'Sale up to 30% OFF — Free shipping on all your order. We deliver, you enjoy',
  imageUrl: ''
}];

export default function Swiper({ slides }) {
  const data = slides.length > 0 ? slides : DEMO;
  const [current, setCurrent] = useState(0);

  const goTo = useCallback((n) => {
    setCurrent((n + data.length) % data.length);
  }, [data.length]);

  useEffect(() => {
    const t = setInterval(() => goTo(current + 1), 4500);
    return () => clearInterval(t);
  }, [current, goTo]);

  const s = data[current];

  return (
    <div className="swiper">
      <div className="swiper-inner">
        <div className="swiper-img-wrap">
          <div className="swiper-img-circle">
            <svg viewBox="0 0 100 100" fill="rgba(255,255,255,0.15)">
              <circle cx="50" cy="35" r="20"/>
              <ellipse cx="50" cy="80" rx="30" ry="20"/>
            </svg>
          </div>
        </div>

        <div className="swiper-text">
          <p className="swiper-top">{s.topText}</p>
          <h1 className="swiper-title">
            {s.bigTitle}<br/>
            <span className="swiper-normal">{s.highlightedTitleNormal} </span>
            <span className="swiper-color">{s.highlightedTitleColor}</span>
            {s.highlightedTitleBold && <strong> {s.highlightedTitleBold}</strong>}
          </h1>
          <p className="swiper-bottom">{s.bottomText}</p>
        </div>
      </div>

      <button className="swiper-btn swiper-prev" onClick={() => goTo(current - 1)}>&#8592;</button>
      <button className="swiper-btn swiper-next" onClick={() => goTo(current + 1)}>&#8594;</button>

      <div className="swiper-dots">
        {data.map((_, i) => (
          <button
            key={i}
            className={`dot ${i === current ? 'active' : ''}`}
            onClick={() => setCurrent(i)}
          />
        ))}
      </div>
    </div>
  );
}
