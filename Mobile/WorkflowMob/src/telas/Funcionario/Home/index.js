import React, { useState, useEffect } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, ActivityIndicator} from 'react-native';
import { Card, Title, Paragraph } from 'react-native-paper';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'

export default function Home({navigation, route}){

  const { theme } = useTheme();
  const styles = getStyles(theme);
  
  const usuario = route.params?.usuario || {
    nome: 'Usuário não identificado',
    cargo: 'Cargo não definido',
  };
    console.log('Dados recebidos na Home:', route.params);

  const [usuarioState, setUsuarioState] = useState(usuario);
  const [imagens, setImagens] = useState([]);
  const [loading, setLoading] = useState(true);


//Mostra a foto do Usuario:
  useEffect(() => {
    if (!usuario.id || !usuario.role) return;

    async function carregarImagens() {
      try {
        const response = await fetch(`http://10.239.0.125/dev4tec/imagem_usuario.php`,{
            method:'POST',
            headers:{'Content-Type': 'application/json'},
            body: JSON.stringify({id: usuario.id, role: usuario.role})
          }
        );
        const data = await response.json();

        if (data.success) {
          setUsuarioState(prev => ({ ...prev, imagem: data.imagem }));
        }
      } catch (error) {
        console.error('Erro ao buscar imagens:', error);
      } finally {
        setLoading(false);
      }
    }

      carregarImagens();
  }, []);

  if (loading) {
    return (
      <View style={styles.loading}>
        <ActivityIndicator size="large" color="#4a90e2" />
        <Text style={styles.loadingText}>Carregando imagens...</Text>
      </View>
    );
  }
  console.log('Imagem do usuário:', data.imagem);

    return(
        <ScrollView style={styles.scroll}>
            <View style={styles.container}>
                <View style={styles.areaperfil}>
                <Image 
                  source={{ uri: usuario.foto_perfil }}
                  style={{ width: 150, height: 150 }}
                />
                    <View style={styles.verde}></View>

                    <View style={styles.textoperfil}>
                        <Text style={styles.nome}>{usuarioState.nome}</Text>
                        <Text style={styles.profissao}>{usuarioState.cargo}</Text>
                    </View>
                </View>

                <View style={styles.areatitulo}>
                    <Text style={styles.titulo}>Home</Text>
                    <Text style={styles.subtitulo}>Explore as Ferramentas</Text>
                </View>

                <View style={styles.areacard}>
                    <TouchableOpacity
                        onPress={()=> navigation.navigate('Equipes')} 
                    >
                        <Card style={styles.cardtarequi}>
                            <Card.Cover 
                                source={require('../../../../assets/img/equipes.png')}
                                style={styles.imagemcard} />
                            <Card.Content style={styles.cardinferior}>
                                <Title style={styles.titulocard}>Equipes</Title>
                                <Paragraph style={styles.paragraph}>Veja suas Equipes</Paragraph>
                                <View style={styles.linhainfer}>
                                    <Text style={styles.data}>16/07/20</Text>
                                    <Text style={styles.Entre}>Entre aqui</Text>
                                </View>
                            </Card.Content>
                        </Card>
                    </TouchableOpacity>
                    
                    <TouchableOpacity
                         onPress={()=> navigation.navigate('Tarefas')} 
                    >
                    <Card style={styles.cardtarequi}>
                        <Card.Cover 
                            source={require('../../../../assets/img/tarefas.png')} 
                            style={styles.imagemcard}/>
                        <Card.Content style={styles.cardinferior}>
                            <Title style={styles.titulocard}>Tarefas</Title>
                            <Paragraph style={styles.paragraph}>Veja suas Tarefas</Paragraph>
                            <View style={styles.linhainfer}>
                                <Text style={styles.data}>16/07/20</Text>
                                <Text style={styles.Entre}>Entre aqui</Text>
                            </View>
                        </Card.Content>
                    </Card>
                    </TouchableOpacity>

                    <TouchableOpacity
                        onPress={()=> navigation.navigate('Ranking')} 
                    >
                    <Card style={styles.cardtarequi}>
                        <Card.Cover 
                            source={require('../../../../assets/img/ranking.png')} 
                            style={styles.imagemcard}/>
                        <Card.Content style={styles.cardinferior}>
                            <Title style={styles.titulocard}>Ranking</Title>
                            <Paragraph style={styles.paragraph}>Veja aqui seus pontos</Paragraph>
                            <View style={styles.linhainfer}>
                                <Text style={styles.data}>16/07/20</Text>
                                <Text style={styles.Entre}>Entre aqui</Text>
                            </View>
                        </Card.Content>
                    </Card>
                    </TouchableOpacity>
                    
                </View>
            </View>
        </ScrollView>
    )
}
