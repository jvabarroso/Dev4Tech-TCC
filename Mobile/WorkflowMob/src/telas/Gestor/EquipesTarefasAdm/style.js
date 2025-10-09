import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const getStyles = (theme) => StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: theme.background,
  },
  scrollContent: {
    padding: 16,
  },
  titulo: {
    fontSize: 30,
    fontFamily: fonts.text,
    color: theme.text,
    fontWeight: 'bold',
    padding: 10,
  },
  subtitulo: {
    fontSize: 20,
    fontFamily: fonts.text,
    color: theme.text,
    fontWeight: 'bold',
    padding: 10,
    paddingHorizontal:18,
  },
  areabotao: {
    padding: 10,
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    alignContent:"space-around"
    
  },
  botao: {
    paddingVertical: 10,
    paddingHorizontal: 15,
    marginHorizontal: 8,
    borderRadius: 15,
    alignItems: 'center',
    justifyContent: 'center'
  },
  textobotao: {
    fontSize: 13,
    fontFamily: fonts.text,
  },
  navinput: {
    width: '100%',
    padding: 10,
    fontSize: 17,
    fontFamily: fonts.text,
    backgroundColor: '#1C58F2',
    borderRadius: 10,
    borderBottomWidth: 0.1,
    borderBottomColor: '#000',
    marginBottom: 15,
    color: '#fff',
  },
  flat: {
    flex: 1,
  },
  containertarefas: {
    backgroundColor: theme.inputBackground,
    borderRadius: 10,
    padding: 15,
    marginBottom: 20,
  },
  linhaTarefa: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 10,
    position: 'relative'
  },
  textosTarefa: {
    marginLeft: 10,
    flexShrink: 1,
  },
  imag: {
    width: 45,
    height: 45,
  },
  textolistatitulo: {
    color: theme.text,
    fontSize: 18,
    fontFamily: fonts.text,
    fontWeight: 'bold',
  },
  textolista: {
    color: theme.text,
    fontSize: 15,
    fontFamily: fonts.text,
  },
  linhaInfo: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 5,
  },
  textolistacargo: {
    color: theme.text,
    fontSize: 13,
    fontFamily: fonts.text,
    backgroundColor: theme.inputBackground2,
    borderRadius: 15,
    paddingHorizontal: 10,
    paddingVertical: 5,
  },
  textolistadata: {
    color: theme.text,
    fontSize: 14,
    fontFamily: fonts.text,
    marginTop:4,
  },
  containerfiltro:{
    position: 'absolute',
    backgroundColor: "#4CAF50",
    borderRadius: 10,
    paddingHorizontal: 5,
    left:260
  },
  textofiltro:{
    color: "#fff",
    fontSize: 13,
    fontFamily: fonts.text,
    padding:5,
  },
  nav: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingVertical: 15,
    marginBottom: 10,
    marginTop:15,
  },
  botaodevoltar: {
    width: 40,
    height: 40,
    justifyContent: 'center',
    marginTop:10,
  },
  tituloi: {
    width:150,
    height:50,
  },
});
