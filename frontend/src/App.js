import React, { useState, useEffect } from 'react';
import TopBar from './components/TopBar';
import Navbar from './components/Navbar';
import NavMenu from './components/NavMenu';
import Swiper from './components/Swiper';
import PopularCategories from './components/PopularCategories';
import Newsletter from './components/Newsletter';
import Footer from './components/Footer';
import Toast from './components/Toast';
import { fetchSlides, fetchPopularCategories, fetchCategories } from './api';

export default function App() {
  const [slides, setSlides]       = useState([]);
  const [popular, setPopular]     = useState([]);
  const [categories, setCategories] = useState([]);
  const [toast, setToast]         = useState(null);
  const lang = 'en';

  useEffect(() => {
    Promise.all([
      fetchSlides(lang),
      fetchPopularCategories(lang),
      fetchCategories(lang)
    ]).then(([s, p, c]) => {
      setSlides(s || []);
      setPopular(p || []);
      setCategories(c || []);
      if (!s && !p && !c) {
        setToast({ msg: 'Could not connect to API. Make sure the backend is running.', error: true });
      }
    });
  }, []);

  return (
    <div>
      <TopBar />
      <Navbar />
      <NavMenu categories={categories} />
      <Swiper slides={slides} />
      <PopularCategories items={popular} />
      <Newsletter />
      <Footer />
      {toast && <Toast message={toast.msg} error={toast.error} onClose={() => setToast(null)} />}
    </div>
  );
}
