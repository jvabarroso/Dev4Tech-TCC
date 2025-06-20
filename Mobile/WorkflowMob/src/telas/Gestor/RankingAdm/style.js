import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const getStyles = (theme) => StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: theme.background,
    paddingHorizontal: 20,
  },
  titulo: {
    fontSize: 30,
    color: theme.text,
    fontWeight: 'bold',
    fontFamily: fonts.text,
    marginTop: '5%',
    marginBottom: "8%",
  },
  navinput: {
    width: '100%',
    padding: 10,
    fontSize: 17,
    backgroundColor: '#1C58F2',
    borderRadius: 10,
    borderBottomWidth: 0.1,
    borderBottomColor: '#000',
    marginBottom: 15,
    color: theme.text,
  },
  flat: {
    flex: 1,
  },
  containertarefas: {
    backgroundColor: theme.inputBackground,
    borderRadius: 10,
    padding: 10,
    marginBottom: 20,
    flexDirection: 'row',
    alignItems: 'center',
  },
  imag: {
    width: 45,
    height: 45,
    marginLeft: 10,
  },
  textos: {
    marginLeft: 15,
    flex: 1,
  },
  textolistatitulo: {
    color: theme.text,
    fontSize: 18,
    fontWeight: 'bold',
    fontFamily: fonts.text,
  },
  textolistacargo: {
    color: theme.text,
    fontSize: 15,
    fontFamily: fonts.text,
  },
  colocacao:{
    fontSize: 28, 
    fontWeight: 'bold',
    color: theme.text, 
    fontFamily: fonts.text,
    marginRight: 10,
  },
});
