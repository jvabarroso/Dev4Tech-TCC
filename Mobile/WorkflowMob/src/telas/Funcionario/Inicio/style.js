import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const getStyles = (theme) => StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: theme.background,
    alignItems: 'center',
    justifyContent: 'center',
  },
  logo: {
    width:200,
    height:200,
    flexDirection: 'row',
    paddingBottom: 5,
    marginBottom: 500,
    marginRight: 160,
  },
  linha:{
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  areaTitulo: {
    position: 'absolute',
    alignContent: 'center',
    top: '30%',
    padding:25,
  },
  titulo: {
    fontFamily: fonts.text,
    fontSize: 35,
    fontWeight: 'bold',
    color: theme.text,
    marginBottom: 10,
    marginRight:100,
    paddingHorizontal:10,
  },
  subtitulo: {
    fontFamily: fonts.text,
    fontSize: 18,
    fontWeight: 'bold',
    color: theme.text,
    paddingHorizontal:10,
  },
  botao:{
    position: 'absolute',
    top: '68%',
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