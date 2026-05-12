const BASE = '';  // uses proxy from package.json -> localhost:5001

async function get(path) {
  try {
    const res = await fetch(BASE + path);
    if (!res.ok) throw new Error('HTTP ' + res.status);
    return await res.json();
  } catch (e) {
    console.warn('API:', path, e.message);
    return null;
  }
}

export const fetchSlides             = (lang) => get(`/api/home-slides/${lang}`);
export const fetchPopularCategories  = (lang) => get(`/api/popular-categories/${lang}`);
export const fetchCategories         = (lang) => get(`/api/categories/${lang}`);
