import React, { useState, useEffect, useRef } from "react";
import {Text, View, TextInput, TouchableOpacity, FlatList, KeyboardAvoidingView, Platform, Image} from "react-native";
import axios from "axios";
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { Ionicons } from '@expo/vector-icons';

import url from '../../../../services/url';

const API_URL = `${url}dev4tech`;

export default function Chat({ route, navigation }) {
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const equipe = route.params?.equipe || {}; 
  const usuario = route.params?.usuario;

  const [texto, setTexto] = useState("");
  const [mensagens, setMensagens] = useState([]);
  const flatListRef = useRef();

  useEffect(() => {
    carregarMensagens();
    marcarComoLido();
    const interval = setInterval(() => {
      carregarMensagens();
      marcarComoLido();
    }, 3000);

    return () => clearInterval(interval);
  },[]);

  const marcarComoLido = async () => {
  try {
    await axios.post(`${API_URL}/marcar_lido.php`, { FuncionarioId: usuario.FuncionarioId });
    carregarMensagens(); // Atualiza mensagens após marcar como lido
  } catch (err) {
    console.log("Erro ao marcar como lido", err);
  }
};

  const carregarMensagens = async () => {
    try {
      const res = await axios.get(`${API_URL}/listarmensagem.php`, {
        params: { id_equipe: equipe.id_equipe }
      });

      const msgs = res.data.reverse();
      setMensagens(msgs);

    } catch (err) {
      console.log("Erro ao buscar mensagens", err);
    }
  };

  const enviarMensagem = async () => {
    if (texto.trim() === "") return;
    try {
      await axios.post(`${API_URL}/enviarmensagem.php`, 
      { 
        Texto: texto,
        id_equipe: equipe.id_equipe,
        FuncionarioId: usuario.FuncionarioId,
        AdminId: null,
        id_empresa: usuario.id_empresa
      });
      setTexto("");
      carregarMensagens();
    } catch (err) {
      console.log("Erro ao enviar mensagem", err);
    }
  };

  // para formatar a hora
  const formatarHora = (dataHora) => {
    if (!dataHora) return "";
    const date = new Date(dataHora);
    return date.toLocaleTimeString("pt-BR", {
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  // retornar o ícone de status 
  const renderStatus = (status) => {
    if (status === "entregue") return <Ionicons name="checkmark-outline" size={24} color={theme.text} />;
    if (status === "lida") return <Ionicons name="checkmark-done-outline" size={24} color={theme.text} />;
    return "";
  };


  return (
    <KeyboardAvoidingView
      style={styles.container}
      behavior={Platform.OS === "ios" ? "padding" : undefined}
    >
    <View style={styles.nav}>
      <TouchableOpacity 
        style={styles.botaodevoltar}
        onPress={() => navigation.goBack()}
      >
        <Ionicons name="arrow-back" size={24} color={theme.text} />
      </TouchableOpacity>
        <Image 
          style={styles.titulo}
          source={theme.logo} >
        </Image>
        <View style={styles.espacoHeader} />
      </View>

      <FlatList
        ref={flatListRef}
        data={mensagens}
        keyExtractor={(item) => item.id_mensagem.toString()}
        renderItem={({ item }) => {
          const isMe = item.FuncionarioId === usuario.FuncionarioId;
          return (
            <View
              style={[
                styles.msg,
                isMe ? styles.msgMinha : styles.msgOutro,
              ]}
            >
              {/* {!isMe && (
                <Text style={styles.usuario}>
                  {userEmojis[item.usuario] || EMOJI_PADRAO} {item.usuario}
                </Text>
              )} */}
              <Text style={styles.texto}>{item.Texto}</Text>

              <View style={styles.linhaHora}>
                <Text style={styles.hora}>{formatarHora(item.data_envio)}</Text>
                {isMe && (
                  <Text
                    style={[
                      styles.status,
                      item.status === "lido" && styles.statusLido,
                    ]}
                  >
                    {renderStatus(item.status)}
                  </Text>
                )}
              </View>
            </View>
          );
        }}
        onContentSizeChange={() =>
          flatListRef.current.scrollToEnd({ animated: true })
        }
        onLayout={() =>
          flatListRef.current.scrollToEnd({ animated: true })
        }
      />

      <View style={styles.inputArea}>
        <TextInput
          style={styles.inputMensagem}
          placeholder="Digite sua mensagem..."
          value={texto}
          onChangeText={setTexto}
        />
        <TouchableOpacity style={styles.botaoEnviar} onPress={enviarMensagem}>
          <Text style={styles.botaoTexto}>➤</Text>
        </TouchableOpacity>
      </View>
    </KeyboardAvoidingView>
  );
}


