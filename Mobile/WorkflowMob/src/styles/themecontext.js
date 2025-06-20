import React, { createContext, useState, useContext } from 'react';
import { lightTheme, darkTheme } from './theme';

const themecontext = createContext();

export const ThemeProvider = ({ children }) => {
  const [isDarkMode, setIsDarkMode] = useState(false);

  const toggleTheme = () => setIsDarkMode(!isDarkMode);

  const theme = isDarkMode ? darkTheme : lightTheme;

  return (
    <themecontext.Provider value={{ theme, isDarkMode, toggleTheme }}>
      {children}
    </themecontext.Provider>
  );
};

export const useTheme = () => useContext(themecontext);
