import React from 'react';
import './PopularCategories.css';

const ICONS = {
  fashion:'👗', skin:'🧴', dish:'🧹', furniture:'🛋️',
  decor:'🌿', electronic:'🎧', book:'📚', perfume:'🌸',
  travel:'🧳', sport:'🏋️', diabetic:'🍬', food:'🛒'
};

function getIcon(title = '') {
  const t = title.toLowerCase();
  for (const [k, v] of Object.entries(ICONS)) {
    if (t.includes(k)) return v;
  }
  return '📦';
}

const DEMO = [
  { id:'1', title:'Fashion' }, { id:'2', title:'Skin Care' },
  { id:'3', title:'Dish Detergents' }, { id:'4', title:'Furniture' },
  { id:'5', title:'Decor' }, { id:'6', title:'Electronic' },
  { id:'7', title:'Books' }, { id:'8', title:'Perfumes' },
  { id:'9', title:'Travel & Leisure' }, { id:'10', title:'Sports' },
  { id:'11', title:'Diabetic Food' }, { id:'12', title:'Food' },
];

export default function PopularCategories({ items }) {
  const data = items.length > 0 ? items : DEMO;

  return (
    <section className="popular-section">
      <div className="section-header">
        <h2 className="section-title">Popular Categories</h2>
        <a href="#" className="view-all">View All &#8594;</a>
      </div>
      <div className="popular-grid">
        {data.map((item, i) => (
          <a key={item.id || i} href="#" className="pop-card" style={{ animationDelay: `${i * 40}ms` }}>
            <div className="pop-icon">{getIcon(item.title)}</div>
            <p className="pop-title">{item.title}</p>
          </a>
        ))}
      </div>
    </section>
  );
}
