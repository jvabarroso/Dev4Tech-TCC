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
    borderRadius: 15,
    paddingHorizontal: 5,
    paddingVertical: 3,
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
  espacoHeader: { 
    width: 40 
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
  botao: {
    paddingVertical: 4,
    paddingHorizontal: 10,
    borderRadius: 8,
    borderWidth: 2,
    borderColor: theme.border,
    backgroundColor: '#E0E0E0' 
  },
  textoBotao: {
    color: '#eeeeeeff',
    fontWeight: 'bold',
    fontSize: 13,
    fontFamily: fonts.text,
  },
  linhaBotoes: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    gap:5,
    bottom:3
  },

  modalOverlay: {
    flex: 1,
    backgroundColor: theme.background,
    justifyContent: 'center',
    alignItems: 'center',
  },
  modalContent: {
    backgroundColor: 'white',
    padding: 20,
    borderRadius: 10,
    width: '85%',
    maxHeight: '80%',
  },
  modalTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    marginBottom: 15,
    textAlign: 'center',
  },
  problemaTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    marginBottom: 10,
    color: '#333',
  },
  problemasScroll: {
    maxHeight: 200,
    marginBottom: 15,
  },
  problemaContainer: {
    backgroundColor: '#FFF3E0',
    padding: 12,
    borderRadius: 8,
    borderLeftWidth: 4,
    borderLeftColor: '#FF9800',
    marginBottom: 10,
  },
  problemaIndex: {
    fontSize: 14,
    fontWeight: 'bold',
    marginBottom: 5,
    color: '#E65100',
  },
  problemaText: {
    fontSize: 14,
    lineHeight: 18,
    color: '#333',
  },
  instrucoesText: {
    fontSize: 14,
    color: '#666',
    marginBottom: 20,
    lineHeight: 20,
    textAlign: 'center',
  },
  modalButtons: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 10,
  },
  modalButton: {
    padding: 12,
    borderRadius: 5,
    minWidth: 120,
    alignItems: 'center',
  },
  modalButtonText: {
    fontWeight: 'bold',
    fontSize: 14,
  },
});
