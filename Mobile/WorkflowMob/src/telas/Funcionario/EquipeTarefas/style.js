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
    color: theme.text,
    fontWeight: 'bold',
    fontFamily: fonts.text,
  },
  titulo2: {
    fontSize: 20,
    color: theme.text,
    fontWeight: 'bold',
    fontFamily: fonts.text,
    padding:10,
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
    marginHorizontal: 6,
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
  containertarefas2: {
    backgroundColor: theme.inputBackground,
    borderRadius: 10,
    padding: 10,
    marginBottom: 20,
    flexDirection: 'row',
    alignItems: 'center',
  },
  linhaTarefa: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 10,
  },
  textosTarefa: {
    marginLeft: 10,
    flexShrink: 1,
  },
  imag: {
    width: 45,
    height: 45,
  },
  textos: {
    marginLeft: 15,
    flex: 1,
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
    paddingHorizontal: 7,
    paddingVertical: 3,
  },
  textolistadata: {
    color: theme.text,
    fontSize: 14,
    fontFamily: fonts.text,
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
  },
  espacoHeader: {
        width: 40,
  },
});
