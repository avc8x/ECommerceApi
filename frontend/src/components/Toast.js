import React, { useEffect } from 'react';
import './Toast.css';

export default function Toast({ message, error, onClose }) {
  useEffect(() => {
    const t = setTimeout(onClose, 4000);
    return () => clearTimeout(t);
  }, [onClose]);

  return (
    <div className={`toast ${error ? 'toast-error' : ''}`}>
      {message}
      <button className="toast-close" onClick={onClose}>✕</button>
    </div>
  );
}
