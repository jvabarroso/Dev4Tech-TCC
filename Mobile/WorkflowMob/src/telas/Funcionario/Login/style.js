import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const  getStyles = (theme) => StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: theme.background,
    padding: 20,
    alignItems: 'center',
    justifyContent: 'center',
  },
  logo: {
    fontSize: 19,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: theme.text,
    alignSelf: 'flex-start',
    marginBottom: "28%",
    marginLeft: "2%",
  },
  titulo: {
    fontSize: 35,
    fontWeight: 'bold',
    fontFamily: fonts.text,
    color: theme.text,
    alignSelf: 'flex-start',
    marginBottom: "10%",
    marginLeft:10,
    marginTop:40,
  },
  area: {
    width: '90%',
    alignItems: 'center',
    alignContent:"center",
    marginBottom: "10%",
  },
  texto: {
    fontSize: 18,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: theme.text2,
    alignSelf:"flex-start",
    paddingHorizontal:10,
  },
  input: {
    width: '100%',
    backgroundColor: theme.inputBackground3,
    color:theme.text,
    padding: 10,
    fontSize: 16,
    borderRadius: 10,
    borderBottomWidth: 1.4,
    borderBottomColor: '#000',
    marginBottom: 15,    
    alignSelf: 'center',
  },
  senha: {
    color: '#0000FF',
    marginBottom: 20,
  },
  botao: {
    height: 45,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#0C21C1',
    borderRadius: 150,
    width: '75%',
  },
  textoBotao: {
    fontSize: 15,
    fontFamily: fonts.text,
    color: '#FFFFFF',
  },
})