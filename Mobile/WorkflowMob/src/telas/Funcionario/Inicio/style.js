import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
  logo: {
    fontSize: 19,
    fontFamily: fonts.text,
    color: '#000',
    fontWeight: 'bold',
    flexDirection: 'row',
    paddingBottom: 5,
    marginBottom: '135%',
    marginRight: '60%',
  },
  areaTitulo: {
    position: 'absolute',
    alignItems: 'center',
    top: '30%',
  },
  titulo: {
    fontFamily: fonts.text,
    fontSize: 35,
    fontWeight: 'bold',
    color: '#000',
    marginBottom: 10,
    paddingHorizontal: 10,
  },
  subtitulo: {
    fontFamily: fonts.text,
    fontSize: 18,
    fontWeight: 'bold',
    color: '#000',
    marginBottom: 10,
    marginRight: 40,
    width:300,
  },
  botao:{
    position: 'absolute',
    top: '63%',
    height: 45,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#0C21C1',
    borderRadius: 150,
    width: '75%',
    alignSelf: 'center',
  },
  textoBotao: {
    fontFamily: fonts.text,
    fontSize: 15,
    color: '#FFFFFF'
  },
})