import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#ffffff",
    paddingHorizontal: 20,
  },
  titulo: {
    fontSize: 30,
    fontFamily: fonts.text,
    color: '#000',
    fontWeight: 'bold',
    marginTop: '5%',
    marginBottom: "10%",
  },
  areabotao: {
    marginBottom: 20,
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center'
  },
  botao: {
    backgroundColor: '#1C58F2',
    paddingVertical: 10,
    paddingHorizontal: 15,
    marginHorizontal: 6,
    borderRadius: 15,
    alignItems: 'center',
    justifyContent: 'center'
  },
  textobotao: {
    fontSize: 15,
    fontFamily: fonts.text,
    color: '#FFFFFF',
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
    backgroundColor: '#F5F7FC',
    borderRadius: 10,
    padding: 15,
    marginBottom: 20,
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
  textolistatitulo: {
    color: '#000',
    fontSize: 18,
    fontFamily: fonts.text,
    fontWeight: 'bold',
  },
  textolista: {
    color: '#000',
    fontSize: 15,
    fontFamily: fonts.text,
  },
  linhaInfo: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 5,
  },
  textolistacargo: {
    color: '#181A1F',
    fontSize: 13,
    fontFamily: fonts.text,
    backgroundColor: '#DDE3F0',
    borderRadius: 15,
    paddingHorizontal: 7,
    paddingVertical: 3,
  },
  textolistadata: {
    color: '#181A1F',
    fontSize: 14,
    fontFamily: fonts.text,
  },
});
