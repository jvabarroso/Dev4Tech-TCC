import { StyleSheet } from 'react-native';
import fonts from '../styles/fonts';

export const getStyles = theme => StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: theme.background,
    alignItems: 'center',
    justifyContent: 'center',
  },
  header: {
    marginRight: 15,
    color: theme.text,
    fontFamily: fonts.text,
  },
});