import { StyleSheet } from "react-native";
import fonts from "../../styles/fonts";



export const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    padding: 20,
    alignItems: 'center',
    justifyContent: 'center',
  },
  logo: {
    fontSize: 19,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: '#000',
    alignSelf: 'flex-start',
    marginBottom: "28%",
    marginLeft: "2%",
  },
  titulo: {
    fontSize: 35,
    fontWeight: 'bold',
    fontFamily: fonts.text,
    color: '#000',
    alignSelf: 'flex-start',
    marginBottom: "10%",
    marginLeft:10,
    marginTop:40,
  },
  area: {
    width: '100%',
    alignItems: 'flex-start',
    marginBottom: "10%",
    marginLeft:20,
  },
  texto: {
    fontSize: 18,
    fontFamily: fonts.text,
    color: '#999999',
    marginBottom: 5,
  },
  input: {
    width: '100%',
    color: '#000842',
    padding: 10,
    fontSize: 16,
    borderRadius: 10,
    borderBottomWidth: 1.4,
    borderBottomColor: '#000',
    backgroundColor: 'transparet',
    marginBottom: 15,    
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