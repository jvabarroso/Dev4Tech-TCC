import React, {useEffect} from "react";
import Routes from "./src/routes";
import FlashMessage from "react-native-flash-message";
import { ThemeProvider } from './src/styles/themecontext';
import AppLoading from 'expo-app-loading';


import {
  useFonts,
  Poppins_400Regular,
  Poppins_600SemiBold,
} from '@expo-google-fonts/poppins';


export default function App() {
  const [fontsLoaded] = useFonts({
    Poppins_400Regular,
    Poppins_600SemiBold,
  });

  if (!fontsLoaded) return <AppLoading />;

  return (
    <>
    <ThemeProvider>
    <Routes />
    <FlashMessage icon="auto" duration={5500} style={{ marginTop: 0 }} />
    </ThemeProvider>
    </>
  );
  }

