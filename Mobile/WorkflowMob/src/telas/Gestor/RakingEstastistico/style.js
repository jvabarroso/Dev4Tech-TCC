import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const getStyles = (theme) => StyleSheet.create({
   container: {
      flex: 1,
      backgroundColor: theme.background,
    },
    scrollView: {
      flex: 1,
    },
    containerConteudo: {
      paddingHorizontal: 20,
      paddingTop: 15,
      paddingBottom: 40,
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
    titulo: {
      fontSize: 18,
      color: theme.text,
      fontFamily: fonts.text,
      fontWeight: 'bold',
      textAlign: 'center',
      flex: 1,
    },
    espacoHeader: {
      width: 40,
    },
    titulossub: {
      fontSize: 30,
      fontFamily: fonts.text,
      color: theme.text,
      fontWeight: 'bold',
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
      borderRadius:12,
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
      fontFamily: fonts.text,
      color: theme.text,
      marginRight: 10,
    },
    containerestatisticas:{
      marginBottom: 20,
    },
    tituloestastisca: {
      fontSize: 30,
      fontFamily: fonts.text,
      color: theme.text,
      fontWeight: 'bold',
      marginRight:150,
    },
    linha:{
      flexDirection: 'row',
      justifyContent: 'space-between',
      marginTop:10,
    },
    titulodetalhes:{
      fontSize: 20,
      fontFamily: fonts.text,
      color: theme.text,
      fontWeight: 'bold',
      paddingHorizontal:5
    },
    containerbarras:{
      marginVertical: 13
    },
    colunagrafico: {
      gap: 10,
      marginRight: 10,
      paddingVertical: 15,
      maxWidth: 120,
    },
    textobarras:{
      fontSize: 14,
      fontFamily: fonts.text,
      color: theme.text,
    },
    color:{
      color:"#0E499E"
    },
    barras:{
      flex: 1, 
      height: 20, 
      backgroundColor: '#e0e0e0', 
      borderRadius: 10
    },
    circleProgressView:{
      flexDirection: 'row', 
      alignItems: 'center', 
      padding:10,
    },
    areapontos:{
      paddingVertical:5,
      paddingHorizontal:20,
    },
    textopontos:{
      fontSize: 16,
      fontFamily: fonts.text,
      color: theme.text,
      marginVertical: 2,
    },
    linhaIconeTexto: {
      flexDirection: 'row',
      alignItems: 'center',
      marginVertical: 4,
      gap: 6, 
  },
    azul:{
      width:10,
      height:10,
      borderRadius:5,
      backgroundColor:"#1C58F2",
    },
    vermelho:{
      width:10,
      height:10,
      borderRadius:5,
      backgroundColor:"#D9534F",
    },
    verde:{
      width:10,
      height:10,
      borderRadius:5,
      backgroundColor:"#5BB14F",
    },
    numberInside:{
      fontFamily: fonts.text,
      color: theme.text,
      fontSize:20,
    },
    areafuncionario:{
      flexDirection: 'row',
      alignItems: 'center', 
      marginBottom: 6
    },
    barra:{
      height: '100%',
      backgroundColor: '#0E499E',
      borderRadius: 10,
    },
    circleProgressView: {
      flexDirection: 'row', 
      alignItems: 'center', 
      padding: 10,
      justifyContent: 'center',
      minHeight: 150,
    },
    
});
